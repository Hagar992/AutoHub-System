namespace AutoHub_System.Controllers
{
    public class CarController : BaseController
    {
        private readonly ICarService _carService;
        private readonly ICloudinaryService _cloudinaryService;

        public CarController(ICarService carService, ICloudinaryService cloudinaryService, ApplicationDbContext context, UserManager<User> userManager)
              : base(userManager)
        {
            _carService = carService;
            _cloudinaryService = cloudinaryService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            var cars = _carService.get_all();
            return View(cars);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //return View();
            ViewBag.Brands = new List<string>
    {
        "BMW","Mercedes","Toyota","Honda","Nissan","Hyundai","Kia","Mazda",
        "Volkswagen","Chevrolet","Opel","Dodge","Tesla","Jeep","GMC",
        "Ferrari","Fiat","Land Rover","Jaguar","Lamborghini"
    };

            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // مهم جدًا تتبعت هنا
                ViewBag.Brands = new List<string>
        {
            "BMW","Mercedes","Toyota","Honda","Nissan","Hyundai","Kia","Mazda",
            "Volkswagen","Chevrolet","Opel","Dodge","Tesla","Jeep","GMC",
            "Ferrari","Fiat","Land Rover","Jaguar","Lamborghini"
        };

                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string mainImageUrl = null;

                    if (viewModel.MainImage != null && viewModel.MainImage.Length > 0)
                    {
                        mainImageUrl = await _cloudinaryService.UploadImageAsync(viewModel.MainImage, "cars");
                    }
                    // Upload images to Cloudinary - ADD FOLDER PARAMETER
                    var imageUrls = new List<string>();
                    if (viewModel.Images != null && viewModel.Images.Count > 0)
                    {
                        foreach (var image in viewModel.Images)
                        {
                            if (image.Length > 0)
                            {
                                // Add the folder parameter - using "cars" folder
                                var imageUrl = await _cloudinaryService.UploadImageAsync(image, "cars");
                                if (!string.IsNullOrEmpty(imageUrl))
                                {
                                    imageUrls.Add(imageUrl);
                                }
                            }
                        }
                    }

                    // Convert ViewModel to Model
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
                        Quantity =viewModel.Quantity,
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
                     ViewBag.Brands = new List<string>
                      {
        "BMW","Mercedes","Toyota","Honda","Nissan","Hyundai","Kia","Mazda",
        "Volkswagen","Chevrolet","Opel","Dodge","Tesla","Jeep","GMC",
        "Ferrari","Fiat","Land Rover","Jaguar","Lamborghini"
                        };

                    ModelState.AddModelError("", $"Error adding car: {ex.Message}");
                }
            }

            // If we got this far, something failed; redisplay form
            return View(viewModel);
        }
        // In CarController - Update the Edit methods
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var car = _carService.find_id(id);
            if (car == null)
            {
                return NotFound();
            }

            // Convert Car to EditCarViewModel
            var viewModel = new EditCarViewModel
            {
                CarID = car.CarID,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Quantity=(int)car.Quantity,
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
            ViewBag.Brands = new List<string>
    {
        "BMW","Mercedes","Toyota","Honda","Nissan","Hyundai","Kia","Mazda",
        "Volkswagen","Chevrolet","Opel","Dodge","Tesla","Jeep","GMC",
        "Ferrari","Fiat","Land Rover","Jaguar","Lamborghini"
    };

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
                ViewBag.Brands = new List<string>
        {
            "BMW","Mercedes","Toyota","Honda","Nissan","Hyundai","Kia","Mazda",
            "Volkswagen","Chevrolet","Opel","Dodge","Tesla","Jeep","GMC",
            "Ferrari","Fiat","Land Rover","Jaguar","Lamborghini"
        };

                var existingCar = _carService.find_id(id);
                viewModel.CarImages = existingCar.CarImage;
                viewModel.Features = existingCar.Feature;

                return View(viewModel);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCar = _carService.find_id(id);
                    // Upload NEW main image if user uploaded one
                    if (viewModel.NewMainImage != null && viewModel.NewMainImage.Length > 0)
                    {
                        existingCar.MainImage = await _cloudinaryService.UploadImageAsync(viewModel.NewMainImage, "cars");
                    }

                    
                    if (existingCar == null)
                    {
                        return NotFound();
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
                    ViewBag.Brands = new List<string>
        {
            "BMW","Mercedes","Toyota","Honda","Nissan","Hyundai","Kia","Mazda",
            "Volkswagen","Chevrolet","Opel","Dodge","Tesla","Jeep","GMC",
            "Ferrari","Fiat","Land Rover","Jaguar","Lamborghini"
        };

                    var existingCar = _carService.find_id(id);
                    viewModel.CarImages = existingCar.CarImage;
                    viewModel.Features = existingCar.Feature;
                    ModelState.AddModelError("", $"Error updating car: {ex.Message}");
                }
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
    }
}