using AutotraderAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography.X509Certificates;

namespace AutotraderAPI.Controllers
{
    [Route("cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        [HttpPost]
        public ActionResult AddNewCar(CreateCarDto createCarDto)
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
                context.Cars.Add(car);
                context.SaveChanges();

                return StatusCode(201, new { result = car, message = "Sikeres felvétel!" });
            }
        }

        [HttpGet]
        public ActionResult GetAllCar()
        {
            using (var context = new AutotraderContext())
            {
                var cars = context.Cars.ToList();

                if (cars != null)
                {
                    return Ok(new { result = cars, message = "Sikeres lekérés!" });
                }
                Exception e = new();
                return BadRequest(new { result = "", message = e.Message });
            }
        }

        [HttpGet("ById")]
        public ActionResult GetCar(Guid id)
        {
            using (var context = new AutotraderContext())
            {
                var car = context.Cars.FirstOrDefault(x => x.Id == id);

                if (car != null)
                {
                    return Ok(new { result = car, message = "Sikeres lekérés!" });
                }
                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });
            }
        }

        [HttpDelete]

        public ActionResult DeleteCar(Guid id)
        {
            using (var context = new AutotraderContext())
            {
                var car = context.Cars.FirstOrDefault(x => x.Id == id);

                if (car != null)
                {
                    context.Cars.Remove(car);
                    context.SaveChanges();

                    return Ok(new { result = car, message = "Sikeres törlés!" });
                }

                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });
            }
        }
        [HttpPut]

        public ActionResult UpdateCar(Guid id, UpdateCarDto updateCarDto)
        {
            using (var context = new AutotraderContext())
            {
                var existingcar = context.Cars.FirstOrDefault(x => x.Id == id);

                if (existingcar != null)
                {
                    existingcar.Brand = updateCarDto.Brand;
                    existingcar.Type = updateCarDto.Type;
                    existingcar.Color = updateCarDto.Color;
                    existingcar.Myear = updateCarDto.Myear;
                    existingcar.UpdatedTime = DateTime.Now;

                    context.Cars.Update(existingcar);
                    context.SaveChanges();

                    return Ok(new { result = existingcar, message = "Sikeres módosítás!" });
                }

                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban!" });
            }

        }
    }
}
