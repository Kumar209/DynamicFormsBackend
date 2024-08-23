using Azure;
using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.RepositoryInterface.Response;
using DynamicFormsBackend.ServiceInterface.Response;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Service.Response
{
    public class FormResponseService : IFormResponseService
    {
        private readonly IFormResponseRepository _responseRepository;
        public FormResponseService(IFormResponseRepository formResponseRepository) 
        {
            _responseRepository = formResponseRepository;
        }


        public async Task<bool> AddFormResponse(FormResponseDto responseDto)
        {
            var responsesString = string.Join(", ", responseDto.Responses.Select(r => $"{r.QuestionID}: {r.Answer}"));

            var mappedEntity = new FormResponse
            {
                FormId = responseDto.FormId,
                Response = responsesString,
                Email = responseDto.Email,
                AnswerMasterId = responseDto.AnswerMasterId ?? null,
                Active = true,
                CreatedOn = DateTime.Now
            };


            var res = await _responseRepository.AddFormResponse(mappedEntity);

            if(res == null)
            {
                return false;
            }

            return true;
        }


        public async Task<IEnumerable<FormResponseDto>> GetAllResponse(int formId)
        {
            var res = await _responseRepository.GetAllResponsesByFormId(formId);

            if(res == null)
            {
                return null;
            }

            var mappedDto = res.Select(r => new FormResponseDto
            {
                Id = r.FormId,
                FormId = r.FormId,
                Response = r.Response,
                Email = r.Email,
                AnswerMasterId = r.AnswerMasterId,

            }).ToList();

            return mappedDto;
        }


        public async Task<FormResponseDto> GetResponse(int responseId)
        {
            var res = await _responseRepository.GetResponseById(responseId);

            if (res == null)
            {
                return null;
            }


            var mappedDto = new FormResponseDto
            {
                Id = res.Id,
                FormId = res.FormId,
                Response = res.Response,
                Email = res.Email,
                AnswerMasterId = res.AnswerMasterId,   
            };

            return mappedDto;
        }


        public async Task<bool> softDeleteResponse(int responseId)
        {
            return await  _responseRepository.removeFormResponse(responseId);
        }
    }
}
