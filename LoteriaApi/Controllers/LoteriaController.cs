using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using LoteriaEntities.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LoteriaApi.Controllers
{
    [Route("api/loteria")]
    [ApiController]
    public class LoteriaController : ControllerBase
    {
        private readonly LottoContext _context;

        public LoteriaController(LottoContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult CriarConcurso(Concurso concurso)
        {
            _context.Concurso.Add(concurso);

            return Ok(concurso);  
        }

        [HttpPost]
        public ActionResult RegistrarApostas(IEnumerable<Aposta> lstApostas)
        {

            foreach (var aposta in lstApostas)
            {
                var concurso = _context.Concurso.FirstOrDefault(d => d.Id == aposta.ID);

                if(concurso == null)
                {
                    return NotFound();
                }

                var jogoRealizado = concurso.GerarJogo(aposta);
                _context.Aposta.Add(jogoRealizado);
            }

            _context.SaveChanges();
            return Ok(lstApostas);
        }

        [HttpPost]
        public ActionResult RegistrarAposta(Aposta aposta)
        {
            var concurso = _context.Concurso.FirstOrDefault(d => d.Id == aposta.ID);

            if (concurso == null)
            {
                return NotFound();
            }

            var jogoRealizado = concurso.GerarJogo(aposta);
            _context.Aposta.Add(jogoRealizado);

            _context.SaveChanges();
            return Ok(aposta);
        }

        [Route("vencedores/{idConcurso:int}")]
        public ActionResult<IEnumerable<Aposta>> GetWinners(int idConcurso)
        {
            var concurso = _context.Concurso.FirstOrDefault(d => d.Id == idConcurso);

            if (concurso == null)
            {
                return NotFound();
            }

            var vencedores = concurso.VerificarGanhadores();
            _context.SaveChanges();

            return Ok(vencedores);
        }

        [Route("sortear/{idConcurso:int}")]
        public ActionResult Sortear(int idConcurso)
        {
            var concurso = _context.Concurso.FirstOrDefault(d => d.Id == idConcurso);

            if (concurso == null)
            {
                return NotFound();
            }

            concurso.SortearNumeros();
            _context.SaveChanges();

            return Ok();
        }







        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
