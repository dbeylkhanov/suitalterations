using System.Collections.Generic;
using System.Threading.Tasks;
using SuitAlterations.Core.Common;
using SuitAlterations.Core.Data;
using SuitAlterations.Core.Entities;

namespace SuitAlterations.Core.Services {
	public class SuitAlterationsService : ISuitAlterationsService {
		private readonly SuitAlterationRepository _suitAlterationRepository;
		private readonly CustomerRepository _customerRepository;

		public SuitAlterationsService(SuitAlterationRepository suitAlterationRepository, CustomerRepository customerRepository) {
			_suitAlterationRepository = suitAlterationRepository;
			_customerRepository = customerRepository;
		}

		public async Task<SuitAlteration> CreateSuitAlteration(SuitAlteration newAlteration) {
			return await _suitAlterationRepository.Create(newAlteration);
		}

		public async Task<SuitAlteration> SetSuitAlterationAsPaid(int alterationId) {
			SuitAlteration alteration = await _suitAlterationRepository.GetBy(alterationId);
			if (alteration == null) {
				throw new SuitAlterationNotFoundException($"Suit alteration with id={alterationId} not found");
			}
			if (alteration.Status != SuitAlterationStatus.Created) {
				throw new InconsistentSuitAlterationStatusException($"Suit alteration record has inconsistent status - {alteration.Status.ToString()}");
			}

			alteration.Status = SuitAlterationStatus.Paid;
			return await _suitAlterationRepository.Update(alteration);
		}

		public async Task<IReadOnlyList<SuitAlteration>> GetSuitAlterationsBy(int customerId) {
			return await _suitAlterationRepository.GetSuitAlterationsBy(customerId);
		}

		public async Task<Customer> CreateCustomer(Customer customer) {
			return await _customerRepository.Create(customer);
		}

		public async Task<IReadOnlyList<Customer>> GetAllCustomers() {
			return await _customerRepository.GetAll();
		}
	}
}