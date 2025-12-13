
namespace AutoHub_System.Controllers
{
    public class CarController : BaseController
    {
        private readonly ICarService _carService;
        private readonly ICloudinaryService _cloudinaryService;
        private static readonly List<string> _brands = new()
        {
            "BMW", "Mercedes", "Toyota", "Honda", "Nissan", "Hyundai", "Kia", "Mazda",
            "Volkswagen", "Chevrolet", "Opel", "Dodge", "Tesla", "Jeep", "GMC",
            "Ferrari", "Fiat", "Land Rover", "Jaguar", "Lamborghini"
        };

        public CarController(ICarService carService, ICloudinaryService cloudinaryService,
                           ApplicationDbContext context, UserManager<User> userManager)
              : base(userManager)
        {
            _carService = carService;
            _cloudinaryService = cloudinaryService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Manage(string brand = "", string status = "", string search = "")
        {
            var allCars = _carService.get_all();
            var filteredCars = allCars.AsEnumerable();

            // Apply brand filter
            if (!string.IsNullOrEmpty(brand))
            {
                filteredCars = filteredCars.Where(c => c.Brand?.Equals(brand, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                filteredCars = filteredCars.Where(c => c.CarSatus?.Equals(status, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                filteredCars = filteredCars.Where(c =>
                    (c.Brand?.Contains(search, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.Model?.Contains(search, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.Color?.Contains(search, StringComparison.OrdinalIgnoreCase) == true) ||
                    (c.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) == true)
                );
            }

            // Pass filter values to view
            ViewBag.SelectedBrand = brand;
            ViewBag.SelectedStatus = status;
            ViewBag.SearchTerm = search;
            ViewBag.Brands = _brands;

            return View(filteredCars.ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Brands = _brands;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = _brands;
                return View(viewModel);
            }

            try
            {
                string mainImageUrl = null;

                if (viewModel.MainImage != null && viewModel.MainImage.Length > 0)
                {
                    mainImageUrl = await _cloudinaryService.UploadImageAsync(viewModel.MainImage, "cars");
                }

                var imageUrls = new List<string>();
                if (viewModel.Images != null && viewModel.Images.Count > 0)
                {
                    foreach (var image in viewModel.Images)
                    {
                        if (image.Length > 0)
                        {
                            var imageUrl = await _cloudinaryService.UploadImageAsync(image, "cars");
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                                imageUrls.Add(imageUrl);
                            }
                        }
                    }
                }

                var car = new Car
                {
                    Brand = viewModel.Brand,
                    Model = viewModel.Model,
                    Year = viewModel.Year,
                    Price = viewModel.Price,
                    Color = viewModel.Color,
                    Mileage = viewModel.Mileage,
                    EngineType = viewModel.EngineType,
                    MainImage = mainImageUrl,
                    Quantity = viewModel.Quantity,
                    Transimission = viewModel.Transmission,
                    Fuel_Economy = viewModel.FuelEconomy,
                    SeatNumer = viewModel.SeatNumber,
                    CarSatus = viewModel.CarStatus,
                    Description = viewModel.Description,
                    CarImage = imageUrls,
                    Feature = viewModel.Features,
                    DateAdded = DateTime.Now
                };

                _carService.Add(car);

                TempData["Success"] = "Car added successfully!";
                return RedirectToAction("Manage", "Car");
            }
            catch (Exception ex)
            {
                ViewBag.Brands = _brands;
                ModelState.AddModelError("", $"Error adding car: {ex.Message}");
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var car = _carService.find_id(id);
            if (car == null)
            {
                return NotFound();
            }

            var viewModel = new EditCarViewModel
            {
                CarID = car.CarID,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Quantity = (int)car.Quantity,
                Price = (decimal)car.Price,
                Color = car.Color,
                Mileage = car.Mileage,
                EngineType = car.EngineType,
                Transmission = car.Transimission,
                FuelEconomy = car.Fuel_Economy,
                SeatNumber = car.SeatNumer,
                CarStatus = car.CarSatus,
                Description = car.Description,
                CarImages = car.CarImage ?? new List<string>(),
                Features = car.Feature ?? new List<string>(),
                MainImage = car.MainImage
            };

            ViewBag.Brands = _brands;
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarViewModel viewModel)
        {
            if (id != viewModel.CarID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Brands = _brands;
                var existingCar = _carService.find_id(id);
                viewModel.CarImages = existingCar.CarImage;
                viewModel.Features = existingCar.Feature;
                return View(viewModel);
            }

            try
            {
                var existingCar = _carService.find_id(id);
                if (existingCar == null)
                {
                    return NotFound();
                }

                // Upload NEW main image if user uploaded one
                if (viewModel.NewMainImage != null && viewModel.NewMainImage.Length > 0)
                {
                    existingCar.MainImage = await _cloudinaryService.UploadImageAsync(viewModel.NewMainImage, "cars");
                }

                // Upload new images if any
                var newImageUrls = new List<string>();
                if (viewModel.NewImages != null && viewModel.NewImages.Count > 0)
                {
                    foreach (var image in viewModel.NewImages)
                    {
                        if (image.Length > 0)
                        {
                            var imageUrl = await _cloudinaryService.UploadImageAsync(image, "cars");
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                                newImageUrls.Add(imageUrl);
                            }
                        }
                    }
                }

                // Update car properties
                existingCar.Brand = viewModel.Brand;
                existingCar.Model = viewModel.Model;
                existingCar.Year = viewModel.Year;
                existingCar.Price = viewModel.Price;
                existingCar.Color = viewModel.Color;
                existingCar.Mileage = viewModel.Mileage;
                existingCar.EngineType = viewModel.EngineType;
                existingCar.Transimission = viewModel.Transmission;
                existingCar.Fuel_Economy = viewModel.FuelEconomy;
                existingCar.SeatNumer = viewModel.SeatNumber;
                existingCar.CarSatus = viewModel.CarStatus;
                existingCar.Description = viewModel.Description;
                existingCar.Quantity = viewModel.Quantity;

                // Combine existing images with new ones
                if (newImageUrls.Any())
                {
                    existingCar.CarImage ??= new List<string>();
                    existingCar.CarImage.AddRange(newImageUrls);
                }

                existingCar.Feature = viewModel.Features;

                _carService.Update(existingCar);
                TempData["Success"] = "Car updated successfully!";
                return RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                ViewBag.Brands = _brands;
                var existingCar = _carService.find_id(id);
                viewModel.CarImages = existingCar.CarImage;
                viewModel.Features = existingCar.Feature;
                ModelState.AddModelError("", $"Error updating car: {ex.Message}");
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var car = _carService.find_id(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _carService.find_id(id);
            if (car != null)
            {
                _carService.Delete(car);
                TempData["Success"] = $"Car '{car.Brand} {car.Model}' has been deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Car not found!";
            }
            return RedirectToAction(nameof(Manage));
        }

        // API endpoints for filtering (optional)
        [HttpGet]
        public IActionResult GetCarsByBrand(string brand)
        {
            var cars = _carService.GetByBrand(brand);
            return Json(cars);
        }

        [HttpGet]
        public IActionResult GetCarsByStatus(string status)
        {
            var cars = _carService.GetCarsByStatus(status);
            return Json(cars);
        }

        [HttpGet]
        public IActionResult SearchCars(string term)
        {
            var cars = _carService.SearchCars(term);
            return Json(cars);
        }
    }
}
