using System;
using AuctionService.Dto;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item); 
        CreateMap<Item, AuctionDto>(); 
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(d => d.Item, opt => opt.MapFrom(src=> src));
        CreateMap<CreateAuctionDto, Item>(); 
        CreateMap<AuctionDto, AuctionCreated>(); 

        
        CreateMap<Auction, AuctionUpdated>(); 
        CreateMap<Auction, AuctionDeleted>(); 
        
    }


}
