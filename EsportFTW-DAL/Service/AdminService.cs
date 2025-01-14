﻿using EsportFTW_DAL.Interface;
using EsportFTW_DAL.Model;
using EsportFTW_DAL.Repository;

namespace EsportFTW_DAL.Service
{
    internal class AdminService : IAdminService
    {
        private readonly AdminRepository _adminRepository = new();

        public IEnumerable<Admin> Get()
        {
            return _adminRepository.Get();
        }

        public Admin Get(int id)
        {
            return _adminRepository.Get(id);
        }

        public bool Add(Admin entity)
        {
            return _adminRepository.Add(entity);
        }

        public bool Update(Admin entity)
        {
            return _adminRepository.Update(entity);
        }

        public bool Delete(int id)
        {
            return _adminRepository.Delete(id);
        }
    }
}
