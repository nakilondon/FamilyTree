using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ReactNet.Models;
using ReactNet.Repository;

namespace ReactNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyTreeController : ControllerBase
    {
        private readonly IFamilyRepository _familyRepository;

        public FamilyTreeController(IFamilyRepository familyRepository)
        {
            _familyRepository = familyRepository;
        }

        [HttpGet("{id}")]
        public PersonDetails Get(string id)
        {
            var returnValues = _familyRepository.GetDetails(id);

            return returnValues;
        }
        
        [HttpGet]
        public IEnumerable<FamilyTreePerson> Get()
        {
            var returnValues = _familyRepository.GetFamilyTree();
            return returnValues;
        }

        [HttpGet("list")]
        public IEnumerable<ListPerson> GetList()
        {
            return _familyRepository.GetList();
        }
    }
}