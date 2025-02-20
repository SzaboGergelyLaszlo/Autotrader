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
                await context.Cars.AddAsync(car);
                await context.SaveChangesAsync();

                return StatusCode(201, new { result = car, message = "Sikeres felvétel!" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCar()
        {
            using (var context = new AutotraderContext())
            {
                var cars = await context.Cars.ToListAsync();

                if (cars != null)
                {
                    return Ok(new { result = cars, message = "Sikeres lekérés!" });
                }
                Exception e = new();
                return BadRequest(new { result = "", message = e.Message });
            }
        }

        [HttpGet("ById")]
        public async Task<ActionResult> GetCar(Guid id)
        {
            using (var context = new AutotraderContext())
            {
                var car = await context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (car != null)
                {
                    return Ok(new { result = car, message = "Sikeres lekérés!" });
                }
                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });
            }
        }

        [HttpDelete]

        public async Task<ActionResult> DeleteCar(Guid id)
        {
            using (var context = new AutotraderContext())
            {
                var car = await context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (car != null)
                {
                    context.Cars.Remove(car);
                    await context.SaveChangesAsync();

                    return Ok(new { result = car, message = "Sikeres törlés!" });
                }

                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });
            }
        }
        [HttpPut]

        public async Task<ActionResult> UpdateCar(Guid id, UpdateCarDto updateCarDto)
        {
            using (var context = new AutotraderContext())
            {
                var existingcar = await context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (existingcar != null)
                {
                    existingcar.Brand = updateCarDto.Brand;
                    existingcar.Type = updateCarDto.Type;
                    existingcar.Color = updateCarDto.Color;
                    existingcar.Myear = updateCarDto.Myear;
                    existingcar.UpdatedTime = DateTime.Now;

                    context.Cars.Update(existingcar);
                    await context.SaveChangesAsync();

                    return Ok(new { result = existingcar, message = "Sikeres módosítás!" });
                }

                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });
            }

        }
    }
}
