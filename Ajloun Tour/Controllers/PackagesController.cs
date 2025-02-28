using Ajloun_Tour.DTOs.ContactDTOs;
using Ajloun_Tour.DTOs2.PackagesDTOS;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackagesRepository _packagesRepository;

        public PackagesController(IPackagesRepository packagesRepository)
        {
            _packagesRepository = packagesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackagesDTO>>> GetPackagesAsync()
        {

            var Packages = await _packagesRepository.GetALLPackages();

            return Ok(Packages);

        }

        [HttpGet("id")]
        public async Task<ActionResult<PackagesDTO>> GetPackageById(int id)
        {

            var Package = await _packagesRepository.GetPackagesById(id);
            return Ok(Package);
        }

        [HttpPost]
        public async Task<ActionResult<PackagesDTO>> AddPackagesAsync([FromForm] CreatePackages createPackages)
        {

            var addPackage = await _packagesRepository.AddPackagesAsync(createPackages);
            return Ok(addPackage);
        }

        [HttpPut("id")]
        public async Task<ActionResult<PackagesDTO>> UpdatePackageAsync(int id, [FromBody] CreatePackages createPackages)
        {

            var updatePackage = await _packagesRepository.UpdatePackagesAsync(id, createPackages);
            return Ok(updatePackage);
        }

        [HttpDelete("id")]
        public async void DeletePackageById(int id)
        {

            await _packagesRepository.DeletePackagesById(id);

        }
    }
}
