using AuctionService.Data;
using AuctionService.Dto;
using AuctionService.Entities;
using AutoMapper;
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
        public AuctionController(AuctionDbContext context, IMapper mapper)
        {
            _context = context; 
            _mapper = mapper; 
            
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                .Include(a => a.Item)
                .OrderBy(a => a.Item.Make)
                .ToListAsync();   

                return _mapper.Map<List<AuctionDto>>(auctions);  
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction= await _context.Auctions
                .Include(a => a.Item)
                .FirstOrDefaultAsync(a => a.Id == id); 

            if (auction == null) return NotFound(); 
            return _mapper.Map<AuctionDto>(auction);

        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto); 
            // TODO: add current user as seller 

            auction.Seller = "test"; 
            _context.Auctions.Add(auction);
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return BadRequest("Could not save changes to the Db"); 

            } 

            return CreatedAtAction(nameof(GetAuctionById), new { id = auction.Id}, _mapper.Map<AuctionDto>(auction)); 

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions
                .Include(a => a.Item)
                .FirstOrDefaultAsync(a => a.Id == id); 

            if (auction == null) return NotFound();

            // TODO: check seller == username 
            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make; 
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model; 
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color; 
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage; 
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year; 

            var results = await _context.SaveChangesAsync() > 0; 

        if (!results)
        {
            return BadRequest("Could not update auction"); 
        }

        return Ok(); 

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
           var auction = await _context.Auctions.FindAsync(id); 

            if (auction == null) return NotFound(); 

            // TODO: check seller == username 

            _context.Auctions.Remove(auction); 
            var result = await _context.SaveChangesAsync() > 0; 

            if (!result)
            {
                return BadRequest("Could not delete auction"); 
            }

            return Ok(); 

        }
    }
}
