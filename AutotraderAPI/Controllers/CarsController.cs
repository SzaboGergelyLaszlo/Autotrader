using AutotraderAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;

namespace AutotraderAPI.Controllers
{
    [Route("cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AutotraderContext _context;

        public CarsController(AutotraderContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewCar(CreateCarDto createCarDto)
        {
            var car = new Car
            {
                Id = Guid.NewGuid(),
                Brand = createCarDto.Brand,
                Type = createCarDto.Type,
                Color = createCarDto.Color,
                Myear = createCarDto.Myear,
            };

            using (var context = new AutotraderContext())
            {
                await _context.Cars.AddAsync(car);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { result = car, message = "Sikeres felvétel!" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCar()
        {

            var cars = await _context.Cars.ToListAsync();

            if (cars != null)
            {
                return Ok(new { result = cars, message = "Sikeres lekérés!" });
            }
            Exception e = new();
            return BadRequest(new { result = "", message = e.Message });

        }

        [HttpGet("ById")]
        public async Task<ActionResult> GetCar(Guid id)
        {

            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

            if (car != null)
            {
                return Ok(new { result = car, message = "Sikeres lekérés!" });
            }
            return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });

        }

        [HttpDelete]

        public async Task<ActionResult> DeleteCar(Guid id)
        {

            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return Ok(new { result = car, message = "Sikeres törlés!" });
            }

            return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });

        }
        [HttpPut]

        public async Task<ActionResult> UpdateCar(Guid id, UpdateCarDto updateCarDto)
        {

            var existingcar = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

            if (existingcar != null)
            {
                existingcar.Brand = updateCarDto.Brand;
                existingcar.Type = updateCarDto.Type;
                existingcar.Color = updateCarDto.Color;
                existingcar.Myear = updateCarDto.Myear;
                existingcar.UpdatedTime = DateTime.Now;

                _context.Cars.Update(existingcar);
                await _context.SaveChangesAsync();

                return Ok(new { result = existingcar, message = "Sikeres módosítás!" });
            }

            return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });


        }
    }
}
