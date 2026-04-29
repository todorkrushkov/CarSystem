using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CarSystem.ViewModels;
using CarSystem.Data.Models;
using CarSystem.Data.Models.Associative;

namespace CarSystem.App.Infrastructure.Helpers
{
	public static class ModelHandler
	{
		private static IMapper _mapper;

		public static void Initialize(IMapper mapper)
		{
			_mapper = mapper;
		}

		public static void ProcessObservableDtoModels<T>(ObservableCollection<T> toModify, ObservableCollection<T> source)
		{
			if (toModify is BulkObservableCollection<T> bulk)
			{
				bulk.ReplaceAll(source);
			}
			else
			{
				toModify.Clear();
				foreach (var item in source)
					toModify.Add(item);
			}
		}

		public static ObservableCollection<ViolationsViewModel> PersonFinesToObservableDto(List<PersonFines> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<ViolationsViewModel>(x)).ToList();

			return new ObservableCollection<ViolationsViewModel>(dtoModels);
		}

		public static ObservableCollection<PersonViewModel> PersonToObservableDto(List<Person> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<PersonViewModel>(x)).ToList();

			return new ObservableCollection<PersonViewModel>(dtoModels);
		}

		public static ObservableCollection<CarViewModel> CarToObservableDto(List<Car> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<CarViewModel>(x)).ToList();

			return new ObservableCollection<CarViewModel>(dtoModels);
		}

		public static ObservableCollection<ViolationViewModel> ViolationsToObservableDto(List<Violation> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<ViolationViewModel>(x)).ToList();

			return new ObservableCollection<ViolationViewModel>(dtoModels);
		}

		public static ObservableCollection<FineViewModel>FinesToObservableDto(List<Fine> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<FineViewModel>(x)).ToList();

			return new ObservableCollection<FineViewModel>(dtoModels);
		}

		public static ObservableCollection<GenderViewModel> GendersToObservableDto(List<Gender> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<GenderViewModel>(x)).ToList();

			return new ObservableCollection<GenderViewModel>(dtoModels);
		}

		public static ObservableCollection<FuelViewModel> FuelsToObservableDto(List<Fuel> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<FuelViewModel>(x)).ToList();

			return new ObservableCollection<FuelViewModel>(dtoModels);
		}

		public static ObservableCollection<EmissionStandartViewModel> EmissionStandartsToObservableDto(List<EmissionStandart> dbRecords)
		{
			var dtoModels = dbRecords.Select(x => _mapper.Map<EmissionStandartViewModel>(x)).ToList();

			return new ObservableCollection<EmissionStandartViewModel>(dtoModels);
		}
	}
}
