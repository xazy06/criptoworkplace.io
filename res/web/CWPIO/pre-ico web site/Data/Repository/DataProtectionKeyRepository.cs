using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pre_ico_web_site.Data
{
    public class DataProtectionKeyRepository : IXmlRepository
    {
        DataProtectionDbContext _dbContext;

        public DataProtectionKeyRepository(DataProtectionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return new ReadOnlyCollection<XElement>(_dbContext.DataProtectionKeys.Select(k => XElement.Parse(k.XmlData)).ToList());
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var entity = _dbContext.DataProtectionKeys.SingleOrDefault(k => k.FriendlyName == friendlyName);
            if (null != entity)
            {
                entity.XmlData = element.ToString();
                _dbContext.DataProtectionKeys.Update(entity);
            }
            else
            {
                _dbContext.DataProtectionKeys.Add(new DataProtectionKey
                {
                    FriendlyName = friendlyName,
                    XmlData = element.ToString()
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
