using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CarSystem.ViewModels;
using CarSystem.Data.Models;
using CarSystem.Data.Models.Associative;

namespace CarSystem.App.Infrastructure
{
	public class ProviderMappingProfile : Profile
	{
		public ProviderMappingProfile()
		{
			// PersonFines -> ViolationsViewModel
			CreateMap<PersonFines, ViolationsViewModel>(MemberList.None)
				.ForMember(dest => dest.PersonFineId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.FinePrice, opt => opt.MapFrom(src => src.Price.ToString() + " €"))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
				.ForMember(dest => dest.EGN, opt => opt.MapFrom(src => src.Person.EGN))
				.ForMember(dest => dest.CardId, opt => opt.MapFrom(src => src.Person.CardId))
				.ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car.Model))
				.ForMember(dest => dest.CarBrand, opt => opt.MapFrom(src => src.Car.Brand))
				.ForMember(dest => dest.CarNumber, opt => opt.MapFrom(src => src.Car.Number))
				.ForMember(dest => dest.FineNumber, opt => opt.MapFrom(src => src.FineNumber));

			// CreateViolationViewModel -> PersonFines
			CreateMap<CreateViolationViewModel, PersonFines>(MemberList.None)
				.ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
				.ForMember(dest => dest.FineId, opt => opt.MapFrom(src => src.FineId))
				.ForMember(dest => dest.FineNumber, opt => opt.MapFrom(src => src.FineNumber))
				.ForMember(dest => dest.LicenceBackOn, opt => opt.MapFrom(src => src.LicenceBackOn))
				.ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
				.ForMember(dest => dest.ViolationId, opt => opt.MapFrom(src => src.ViolationId))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

			// Person -> PersonViewModel
			CreateMap<Person, PersonViewModel>(MemberList.None)
				.ForMember(dest => dest.EGN, opt => opt.MapFrom(src => src.EGN))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
				.ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName + " - " + src.EGN));

			// Fine -> FineViewModel
			CreateMap<Fine, FineViewModel>(MemberList.None)
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Violation, opt => opt.MapFrom(src => src.Violation));

			// Violation -> ViolationViewModel
			CreateMap<Violation, ViolationViewModel>(MemberList.None)
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

			// Car -> CarViewModel
			CreateMap<Car, CarViewModel>(MemberList.None)
				.ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
				.ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Brand + " " + src.Model + " - " + src.Number))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
				.ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number));

			// Gender -> GenderViewModel
			CreateMap<Gender, GenderViewModel>(MemberList.None)
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

			// Fuel -> FuelViewModel
			CreateMap<Fuel, FuelViewModel>(MemberList.None)
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

			// EmissionStandart -> EmissionStandartViewModel
			CreateMap<EmissionStandart, EmissionStandartViewModel>(MemberList.None)
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
		}
	}
}
