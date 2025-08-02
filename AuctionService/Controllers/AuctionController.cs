using AuctionService.Data;
using AuctionService.Dto;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {

        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndPoint; 
        public AuctionController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint) 
        {
            _context = context;
            _mapper = mapper;
            _publishEndPoint = publishEndpoint; 

        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
        {
            var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

            // var auctions = await _context.Auctions
            //     .Include(a => a.Item)
            //     .OrderBy(a => a.Item.Make)
            //     .ToListAsync();

            // return _mapper.Map<List<AuctionDto>>(auctions);

            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Item)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null) return NotFound();
            return _mapper.Map<AuctionDto>(auction);

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            

            auction.Seller = User.Identity?.Name; 
            Console.WriteLine("user name: " + User.Identity?.Name); 
            _context.Auctions.Add(auction);

            var newAuction = _mapper.Map<AuctionDto>(auction); 


            await _publishEndPoint.Publish(_mapper.Map<AuctionCreated>(newAuction)); 
          
            var result = await _context.SaveChangesAsync() > 0;

              if (!result)
            {
                return BadRequest("Could not save changes to the Db");

            }

            

            return CreatedAtAction(nameof(GetAuctionById), new { id = auction.Id }, newAuction);

        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions
                .Include(a => a.Item)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null) return NotFound();

            if (auction.Seller != User.Identity.Name) return Forbid(); 


            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            await _publishEndPoint.Publish(_mapper.Map<AuctionUpdated>(auction)); 

            var results = await _context.SaveChangesAsync() > 0;

            if (!results)
            {
                return BadRequest("Could not update auction");
            }

            return Ok(_mapper.Map<AuctionDto>(auction));

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if (auction == null) return NotFound();

             

            if (auction.Seller != User.Identity.Name) return Forbid();

            _context.Auctions.Remove(auction);

            await _publishEndPoint.Publish(new AuctionDeleted{ Id = auction.Id.ToString()}); 
            var result = await _context.SaveChangesAsync() > 0;
            
            Console.WriteLine($"Result:{result}");



            if (!result)
            {
                return BadRequest("Could not delete auction");
            }

            return Ok();

        }
    }

    
}
