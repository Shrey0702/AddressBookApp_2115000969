using AutoMapper;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            //AutoMapping from AddressBookDto to AddressBookEntity is done using this code
            CreateMap<AddressBookDTO, AddressBookEntity>().ReverseMap(); // this mapping can work in reverse also
            CreateMap<AddressBookEntity, CreateContactDTO>();
            CreateMap<UserRegistrationDTO, UserEntity>();
            CreateMap<UserEntity, RegisterResponseDTO>();
        }
    }
}
