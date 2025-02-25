﻿using AutoMapper;
using Data.Interfaces.DataConnector;
using Data.Interfaces.Redis;
using Domain.DTO.Usuario;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisIntegrator _cache;

        public UsuarioService(IUsuarioRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IRedisIntegrator cache)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<bool> Delete(int id)
        {
            await Find(id);
            var delete = await _repository.Delete(id);
            return delete;

        }

        public async Task<UsuarioDTO> Get(int id)
        {
            var entity = await Find(id);
            return _mapper.Map<UsuarioDTO>(entity);
        }

        public async Task<IEnumerable<UsuarioDTO>> Get()
        {
            IEnumerable<UsuarioDTO> usuariosCache = await _cache.GetListAsync<UsuarioDTO>("Usuarios");
            if (usuariosCache == null)
            {
                var list = await _repository.Get();
                _cache.SetList("Usuarios", list);
                return _mapper.Map<IEnumerable<UsuarioDTO>>(list);
            }

            return _mapper.Map<IEnumerable<UsuarioDTO>>(usuariosCache);
        }

        public async Task<UsuarioDTO> Post(CriarUsuarioDTO data)
        {
            var model = _mapper.Map<UsuarioModel>(data);
            model.Validate();
            var entity = _mapper.Map<Usuario>(model);
            var result = await _repository.Post(entity);
            _cache.Remove("Usuarios");
            _unitOfWork.CommitTransaction();
            return _mapper.Map<UsuarioDTO>(result);
        }

        public async Task<UsuarioDTO> Put(int id, AlterarUsuarioDTO data)
        {
            var entity = await Find(id);
            var model = _mapper.Map<UsuarioModel>(entity);
            model.Validate();
            _mapper.Map(data, model);
            _mapper.Map(model, entity);
            var result = await _repository.Put(id, entity);
            _cache.Remove("Usuarios");
            return _mapper.Map<UsuarioDTO>(result);
        }

        // Fazer um método genério para que verifique e retorne se a entidade<T> existe, caso não estoure exception
        private async Task<Usuario> Find(int id)
        {
            var entity = _mapper.Map<Usuario>(await _repository.Get(id));
            if (entity == null)
                throw new DomainException("Registro não encontrado", 404);
            return entity;
        }
    }
}
