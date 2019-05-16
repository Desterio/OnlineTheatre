using System.Linq;
using Api.Utils;
using Logic.Common;
using Logic.Customers;
using Logic.Movies;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : BaseController
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;

        public CustomersController(MovieRepository movieRepository, CustomerRepository customerRepository,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _movieRepository = movieRepository;
            _customerRepository = customerRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(long id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return NotFound();

            var dto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name.Value,
                Email = customer.Email.Value,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.Type.ToString(),
                StatusExpirationDate = customer.Status.ExpirationDate,
                PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
                {
                    Price = x.Price,
                    ExpirationDate = x.ExpirationDate,
                    PurchaseDate = x.PurchaseDate,
                    Movie = new MovieDto
                    {
                        Id = x.Movie.Id,
                        Name = x.Movie.Name
                    }
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetList();
            var dtos = customers.Select(x => new CustomerInListDto
            {
                Id = x.Id,
                Name = x.Name.Value,
                Email = x.Email.Value,
                Status = x.Status.Type.ToString(),
                MoneySpent = x.MoneySpent,
                StatusExpirationDate = x.Status.ExpirationDate
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CreateCustomerDto customerModel)
        {
            var customerNameOrError = CustomerName.Create(customerModel.Name);
            var customerEmailOrError = CustomerEmail.Create(customerModel.Email);
            
            var result = Result.Combine(customerNameOrError, customerEmailOrError);
            if (result.IsFailure)
                return Error(result.Error);

            if (_customerRepository.GetCustomerByEmail(customerEmailOrError.Value) != null)
                return Error($"[{customerModel.Email}] is already in use.");

            var customer = new Customer(customerNameOrError.Value, customerEmailOrError.Value);

            _customerRepository.Add(customer);

            return Ok();

            // return CreatedAtAction(nameof(GetCustomerById), new {id = customerModel.Id}, customerModel);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(long id, UpdateCustomerDto customerModel)
        {
            var customerNameOrError = CustomerName.Create(customerModel.Name);

            if (customerNameOrError.IsFailure)
                return Error(customerNameOrError.Error);

            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return Error($"[{id}] is an invalid customer id");

            customer.Name = customerNameOrError.Value;

            return Ok(customer);
        }

        [HttpPost("{id}/movies")]
        public IActionResult PurchaseMovie(long customerId, [FromBody] long movieId)
        {
            var movie = _movieRepository.GetById(movieId);
            if (movie == null)
                return Error($"[{movieId}] is an invalid movie id.");

            var customer = _customerRepository.GetById(customerId);
            if (customer == null)
                return Error($"[{customerId}] is an invalid customer id.");

            if (customer.HasPurchasedMovie(movie))
                return Error($"The movie [{movie.Name}] is already purchased.");

            customer.PurchaseMovie(movie);

            return Ok();
        }

        [HttpPost("{id}/promote")]
        public IActionResult PromoteCustomer(long customerId)
        {
            var customer = _customerRepository.GetById(customerId);
            if (customer == null)
                return Error($"[{customerId}] is an invalid customer id");

            var promotionCheck = customer.CanPromote();
            if (promotionCheck.IsFailure)
                return Error(promotionCheck.Error);
            
            customer.Promote();

            return Ok();
        }
    }
}