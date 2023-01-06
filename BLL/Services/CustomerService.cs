using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CustomerDto model)
        {
            var customer = _mapper.Map<Customer>(model);

            _unitOfWork.CustomerRepository.Add(customer);

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(string modelId)
        {
            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var allBooks = await _unitOfWork.CustomerRepository.GetAllAsync();

            var allBooksDto = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(allBooks);

            return allBooksDto;
        }

        public async Task<CustomerDto> GetByIdAsync(string id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);

            var CustomerDto = _mapper.Map<CustomerDto>(customer);

            return CustomerDto;

        }

        public async Task UpdateAsync(CustomerDto model)
        {
            var customer = _mapper.Map<Customer>(model);

            _unitOfWork.CustomerRepository.Update(customer);

            await _unitOfWork.SaveAsync();
        }
    }
}
