using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Models.Dto.ResultDto;
using BookStore.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public BookController(ApplicationContext context)
        {
            _context = context;
        }

        public CollectionResultDto<BookDto> GetCategories()
        {
            var categories = _context.Books.Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Year = c.Year,
                Author = c.Author,
                Category = c.Category.Name
            }).ToList();
            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = categories
            };
        }

        [HttpGet("BookCategory/{c}")]
        public CollectionResultDto<BookDto> GetBookCategory([FromRoute] string c)
        {

            var categories = _context.Books.Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Year = c.Year,
                Author = c.Author,
                Category = c.Category.Name
            }).Where(x => x.Category == c).ToList();
            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = categories
            };
        }


        [HttpDelete]
        public ResultDto DeleteBook(int id)
        {
            try
            {
                if (id != null)
                {
                    var c = _context.Books.Find(id);
                    _context.Books.Remove(c);
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccessful = true,
                        Message = "Successfully deleted"
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        IsSuccessful = false,
                        Message = "Id is not defined"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = "Something goes wrong"
                };
            }

        }

        [HttpPost]
        [Route("Add")]
        public ResultDto AddBook([FromBody] BookDto dto)
        {
            try
            {
                if (dto != null)
                {
                    Book newC = new Book()
                    {
                        Name = dto.Name,
                        Year = dto.Year,
                        Author = dto.Author,
                        Category = _context.Categories.FirstOrDefault(c => c.Name == dto.Category)
                    };
                    _context.Books.Add(newC);
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccessful = false,
                        Message = "Successfully added"
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        IsSuccessful = false,
                        Message = "Null"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = "Error"
                };
                throw;
            }
        }
        [HttpPost]
        [Route("Update")]
        public ResultDto UpdateBook([FromBody] BookDto dto)
        {
            try
            {
                Book f = _context.Books.First(x => x.Id == dto.Id);
                f.Name = dto.Name;
                f.Year = dto.Year;
                f.Author = dto.Author;
                f.Category = _context.Categories.FirstOrDefault(c => c.Name == dto.Category);
                _context.SaveChanges();
                return new ResultDto
                {
                    IsSuccessful = true,
                    Message = "Successfully created"
                };
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = "Something goes wrong!"
                };
                throw;
            }
        }
    }
}