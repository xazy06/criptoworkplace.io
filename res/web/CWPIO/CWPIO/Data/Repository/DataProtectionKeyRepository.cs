using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWPIO.Data
{
    public class DataProtectionKeyRepository : IXmlRepository
    {
        ApplicationDbContext _dbContext;

        public DataProtectionKeyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return new ReadOnlyCollection<XElement>(_dbContext.Set<DataProtectionKey>().Select(k => XElement.Parse(k.XmlData)).ToList());
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var entity = _dbContext.Set<DataProtectionKey>().SingleOrDefault(k => k.FriendlyName == friendlyName);
            if (null != entity)
            {
                entity.XmlData = element.ToString();
                _dbContext.Set<DataProtectionKey>().Update(entity);
            }
            else
            {
                _dbContext.Set<DataProtectionKey>().Add(new DataProtectionKey
                {
                    FriendlyName = friendlyName,
                    XmlData = element.ToString()
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
