using LoteriaApi;
using LoteriaEntities.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace LoteriaAPiTest
{
    public class LoteriaApiMegaSenaTest
    {
        [Fact]
        public void CriarApostaNumerosAleatorios()
        {
            var concurso = CriaConcurso(1);
            var aposta = CriaApostaSemNumeros(concurso.Id);

            var resultado = concurso.GerarJogo(aposta);

            Assert.Equal(resultado.Concurso, aposta.Concurso);
            ValidaAposta(aposta);
            Assert.Equal(1, concurso.Apostas.Count);
            Assert.Equal(concurso.Apostas[0], aposta);           
        }

        [Fact]
        public void CriarApostaNumerosSelecionados()
        {
            var concurso = CriaConcurso(1);
            var aposta = CriaApostaSemNumeros(concurso.Id);

            aposta.NumerosJogados = new HashSet<int> { 1,2,3,4,5,6};

            var resultado = concurso.GerarJogo(aposta);

            Assert.Equal(resultado.Concurso, aposta.Concurso);
            ValidaAposta(aposta);
            Assert.Equal(1, concurso.Apostas.Count);
            Assert.Equal(concurso.Apostas[0], aposta);
        }

        [Fact]
        public void VerificarGanhadores()
        {
            var concurso = CriaConcurso(1);

            var aposta = CriaApostaSemNumeros(concurso.Id);
            aposta.NumerosJogados = new HashSet<int> { 1, 2, 3, 4, 5, 6 };
            var resultado = concurso.GerarJogo(aposta);

            aposta = CriaApostaSemNumeros(concurso.Id);
            aposta.NumerosJogados = new HashSet<int> { 1, 2, 3, 4, 5, 7 };
            concurso.GerarJogo(aposta);

            aposta = CriaApostaSemNumeros(concurso.Id);
            aposta.NumerosJogados = new HashSet<int> { 1, 2, 3, 4, 8, 7 };
            concurso.GerarJogo(aposta);

            aposta = CriaApostaSemNumeros(concurso.Id);
            aposta.NumerosJogados = new HashSet<int> { 1, 2, 3, 9, 8, 7 };
            concurso.GerarJogo(aposta);

            concurso.NumerosSorteados = new HashSet<int> { 1, 2, 3, 4, 5, 6 };

            var ganhadores = concurso.VerificarGanhadores();

            Assert.Equal(3, ganhadores.Count);
            Assert.Equal(6, ganhadores[0].Acertos);
            Assert.Equal(5, ganhadores[1].Acertos);
            Assert.Equal(4, ganhadores[2].Acertos);
        }

        [Fact]
        public void Sortear()
        {
            var concurso = CriaConcurso(1);

            concurso.SortearNumeros();

            Assert.Equal(6, concurso.NumerosSorteados.Count);
         }

        internal void ValidaAposta(Aposta aposta)
        {
            foreach (int i in aposta.NumerosJogados)
            {
                Assert.InRange(i, aposta.Tipo.NumeroMin, aposta.Tipo.NumeroMax);
            }

            Assert.Equal(aposta.Tipo.QtdSorteada, aposta.NumerosJogados.Count);            
        }

        internal Concurso CriaConcurso(int id)
        {
            Concurso concurso = new Concurso();
            concurso.Id = id;
            concurso.Loteria = new MegaSena();
            concurso.Apostas = new List<Aposta>();

            return concurso;
        }

        internal Aposta CriaApostaSemNumeros(int concursoId)
        {
            Aposta aposta = new Aposta();
            aposta.Concurso = concursoId;
            aposta.Tipo = new MegaSena();
            aposta.DataAposta = DateTime.Now;
            aposta.NumerosJogados = new HashSet<int>();

            return aposta;
        }
    }
}
