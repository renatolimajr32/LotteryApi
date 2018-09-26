using LoteriaEntities.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LoteriaApi
{
    public class Concurso
    {
        public int Id { get; set; }

        [Required]
        public IRegrasLoteria Loteria { get; set; }

        [Required]
        public IList<Aposta> Apostas { get; set; }

        public HashSet<int> NumerosSorteados { get; set; }

        public void SortearNumeros()
        {
            Random random = new Random();

            if(NumerosSorteados == null)
            {
                NumerosSorteados = new HashSet<int>();
            }

            while (NumerosSorteados.Count < Loteria.QtdSorteada)
            {
                NumerosSorteados.Add(random.Next(Loteria.NumeroMin, Loteria.NumeroMax));
            }
        }

        public List<Aposta> VerificarGanhadores()
        {
            if (!NumerosSorteados.Any())
            {
                throw new Exception("Concurso ainda não foi sorteado.");
            }

            List<Aposta> ganhadores = new List<Aposta>();

            foreach (Aposta aposta in Apostas)
            {
                // verifica quantos números a aposta acertou
                var result = aposta.NumerosJogados.Select(i => i).Intersect(NumerosSorteados);
                aposta.Acertos = result.Count();
                
                // verifica se a quantidade de acerto está definida nos ganhadores
                if(Loteria.Ganhadores.Contains(aposta.Acertos))
                {
                    ganhadores.Add(aposta);
                }               
            }

            return ganhadores;
        }

        public Aposta GerarJogo(Aposta jogo)
        {
            if (!ValidarAposta(jogo))
            {
                throw new Exception("Jogo inválido.");
            }

            if (jogo.NumerosJogados.Count == 0)
            {
                GerarSurpresinha(jogo);                 
            }  

            Apostas.Add(jogo);
            return jogo;
        }

        private Aposta GerarSurpresinha(Aposta jogo)
        {
            Random random = new Random();

            while (jogo.NumerosJogados.Count < Loteria.QtdSorteada)
            {
                jogo.NumerosJogados.Add(random.Next(Loteria.NumeroMin, Loteria.NumeroMax));
            }      

            return jogo;    
        }

        private bool ValidarAposta(Aposta jogo)
        {
            bool resultado = true;

            if (jogo.NumerosJogados.Any())
            {
                if (jogo.NumerosJogados.Count != Loteria.QtdSorteada)
                {
                    resultado = false;
                }

                if (jogo.NumerosJogados.Min() < Loteria.NumeroMin)
                {
                    resultado = false;
                }

                if (jogo.NumerosJogados.Max() > Loteria.NumeroMax)
                {
                    resultado = false;
                }

                var dups = jogo.NumerosJogados.GroupBy(x => x)
                   .Where(x => x.Count() > 1)
                   .Select(x => x.Key)
                   .ToList();

                // verificando duplicados
                if (dups.Any())
                {
                    resultado = false;
                }
            }       

            if(jogo.DataAposta == DateTime.MinValue)
            {
                resultado = false;
            }

            if(jogo.Concurso != Id)
            {
                resultado = false;
            }

            return resultado;
        }
    }
}