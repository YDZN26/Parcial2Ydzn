using CadParcial2Ydzn;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnParcial2Ydzn
{
    public class SerieCln
    {
        public static int Insertar(Serie serie)
        {
            using (var context = new Parcial2YdznEntities())
            {
                context.Serie.Add(serie);
                context.SaveChanges();
                return serie.id;
            }
        }

        public static void Actualizar(Serie serie)
        {
            using (var context = new Parcial2YdznEntities())
            {
                var existente = context.Serie.Find(serie.id);
                if (existente != null)
                {
                    existente.titulo = serie.titulo;
                    existente.sinopsis = serie.sinopsis;
                    existente.director = serie.director;
                    existente.episodios = serie.episodios;
                    existente.fechaEstreno = serie.fechaEstreno;
                    existente.estado = serie.estado;
                    existente.urlTrailer = serie.urlTrailer;
                    existente.idiomaOriginal = serie.idiomaOriginal;


                    context.SaveChanges(); // Esto es vital
                }
            }
        }

        public static void Eliminar(int id)
        {
            using (var db = new Parcial2YdznEntities())
            {
                var serie = db.Serie.FirstOrDefault(s => s.id == id);
                if (serie != null)
                {
                    db.Serie.Remove(serie);
                    db.SaveChanges();
                }
            }
        }


        public static Serie ObtenerUno(int id)
        {
            using (var context = new Parcial2YdznEntities())
            {
                return context.Serie.FirstOrDefault(s => s.id == id && s.estado != -1);
            }
        }

        public static List<SerieDto> Buscar(string filtro)
        {
            using (var context = new Parcial2YdznEntities())
            {
                return context.Serie
                    .Where(s => s.estado != -1 && s.titulo.Contains(filtro))
                    .ToList()
                    .Select(s => new SerieDto
                    {
                        id = s.id,
                        titulo = s.titulo,
                        sinopsis = s.sinopsis,
                        director = s.director,
                        episodios = s.episodios.GetValueOrDefault(),
                        fechaEstreno = s.fechaEstreno.GetValueOrDefault(),
                        estadoTexto = s.estado.GetValueOrDefault() == 1 ? "En emisión" :
                                      s.estado.GetValueOrDefault() == 0 ? "Estreno" :
                                      "Cancelada",
                        urlTrailer = s.urlTrailer,
                        idiomaOriginal = s.idiomaOriginal
                    })
                    .ToList();
            }
        }

        public static List<SerieDto> Listar()
        {
            using (var context = new Parcial2YdznEntities())
            {
                return context.Serie
                    .Where(s => s.estado != -1)
                    .ToList()
                    .Select(s => new SerieDto
                    {
                        id = s.id,
                        titulo = s.titulo,
                        sinopsis = s.sinopsis,
                        director = s.director,
                        episodios = s.episodios.GetValueOrDefault(),
                        fechaEstreno = s.fechaEstreno.GetValueOrDefault(),
                        estadoTexto = s.estado == 1 ? "En emisión" :
                                      s.estado == 0 ? "Estreno" :
                                      "Cancelada",
                        urlTrailer = s.urlTrailer,
                        idiomaOriginal = s.idiomaOriginal
                    })
                    .ToList();
            }
        }
    }
}
