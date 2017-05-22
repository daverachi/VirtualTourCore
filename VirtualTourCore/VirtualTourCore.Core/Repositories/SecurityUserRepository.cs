using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Common.Utilities;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Repositories
{
    public class SecurityUserRepository : BaseRepository<SecurityUser>, ISecurityUserRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public SecurityUserRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public IEnumerable<SecurityUser> Get()
        {
            var areas = GetQueryable();
            if (areas == null)
            {
                _log.Warn("Failed to retrieve areas ", new ArgumentException());
            }
            return areas;
        }

        public SecurityUser GetById(int Id)
        {
            var area = GetQueryable()
                .FirstOrDefault(m => m.Id == Id);
            if (area == null)
            {
                _log.Warn("Failed to retrieve by id: " + Id, new ArgumentException());
            }
            return area;
        }

        public SecurityUser GetByUsername(string username)
        {
            var area = GetQueryable()
                .FirstOrDefault(m => m.UserName == username);
            if (area == null)
            {
                _log.Warn("Failed to retrieve by username: " + username, new ArgumentException());
            }
            return area;
        }

        public int? Create(SecurityUser securityUser)
        {
            int? id = null;
            if (securityUser != null)
            {
                if (ValidatePasswordsBeforeHash(securityUser))
                {
                    _log.Info("Creating securityUser");
                    securityUser.PasswordHash = PasswordStorage.CreateHash(securityUser.PasswordPlaintext);
                    base.Add(securityUser);
                    int unit = _UnitOfWork.SaveChanges();
                    if (unit < 1 || securityUser.Id < 1)
                    {
                        _log.Warn("Failed to Create securityUser", new ArgumentNullException());
                    }
                    else
                    {
                        _log.Info("Created Security User with ID " + securityUser.Id);
                        id = securityUser.Id;
                    }
                }
                else
                {
                    _log.Info("Passwords are invalid, Security User not created");
                }
            }
            return id;
        }
        private bool ValidatePasswordsBeforeHash(SecurityUser user)
        {
            bool isValid = false;
            if (user != null)
            {
                string password1 = user.PasswordPlaintext;
                string password2 = user.PasswordPlaintextConfirm;
                int minimumPasswordLength = 6;
                if (!string.IsNullOrWhiteSpace(password1) &&
                    !string.IsNullOrWhiteSpace(password2) &&
                    password1.Equals(password2) &&
                    password1.Length >= minimumPasswordLength &&
                    password1.Any(c => char.IsUpper(c)))
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        public SecurityUser Update(SecurityUser area)
        {
            int? id = null;
            if (area != null)
            {
                _log.Info("Update Area: " + area.UserName + " ID: " + area.UserName);
                base.Update(area);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1)
                {
                    _log.Warn("Failed to Update Area: " + area.UserName + " ID: " + area.UserName, new ArgumentNullException());
                }
            }
            return area;
        }

        public int Delete(int id)
        {
            int deleteId = id;
            _log.Info("Deleting Area with ID: " + id);
            SecurityUser area = GetById(id);
            if (area != null)
            {
                base.Delete(area);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1)
                {
                    deleteId = unit;
                    _log.Warn("Failed to Delete Area with ID: " + id, new ArgumentNullException());
                }
            }
            return id;
        }

        public SecurityUser GetByLogin(string username, string password)
        {
            SecurityUser securityUser;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                securityUser = null;
            }
            else
            {
                securityUser = GetQueryable()
                    .Where(u => u.UserName == username)
                    //.Where(u => u.UserStatusId == (int)UserStatusEnum.ACTIVE)
                    .SingleOrDefault();
                if (securityUser != null)
                {
                    bool passwordMatched = PasswordStorage.VerifyPassword(password, securityUser.PasswordHash);
                    if (!passwordMatched)
                    {
                        securityUser = null;
                    }
                }
            }

            if (securityUser == null)
            {
                _log.Warn("Failed to retrieve user with login credentials");
            }
            return securityUser;
        }
    }
}

