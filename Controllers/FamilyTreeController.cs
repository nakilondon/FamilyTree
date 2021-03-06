﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReactNet.Models;
using ReactNet.Repositories;

namespace ReactNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyTreeController : ControllerBase
    {
        private readonly IFamilyRepository _familyRepository;
        private readonly IImagesRepository _imagesRepository;

        public FamilyTreeController(IFamilyRepository familyRepository,
            IImagesRepository imagesRepository)
        {
            _familyRepository = familyRepository;
            _imagesRepository = imagesRepository;
        }

        [HttpGet("{id}")]
        public async Task<PersonDetails> Get(int id)
        {
            var returnValues = await _familyRepository.GetDetails(id);

            return returnValues;
        }

        [HttpGet]
        public async Task<IEnumerable<FamilyTreePerson>> Get()
        {
            var returnValues = await _familyRepository.GetFamilyTree();
            return returnValues;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<ListPerson>> GetList()
        {
            return await _familyRepository.GetList();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Post([FromForm] FIleUpload fileUpload)
        {
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await fileUpload.File.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var imageData = new ImageData
            {
                FileName = fileUpload.File.FileName,
                Type = fileUpload.File.ContentType,
                Description = fileUpload.Description,
                Location = "",
                Image = fileBytes
            };

            await _imagesRepository.SaveImage(imageData);

            return Ok();
        }

        [HttpGet("img/{fileName}")]
        public async Task<IActionResult> GetImg(string fileName)
        {
            var imageData = await _imagesRepository.GetImage(fileName, ImageType.Original);

            return File(imageData.Image, imageData.Type);
        }

        [HttpGet("thumbnail/{fileName}")]
        public async Task<IActionResult> GetThumbnail(string fileName)
        {
            var imageData = await _imagesRepository.GetImage(fileName, ImageType.Thumbnail);

            return File(imageData.Image, imageData.Type);

        }
    }
}