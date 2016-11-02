using System;
using UrlShortener.Data;

namespace UrlShortener.Web.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly UrlShortenerEntities _context = new UrlShortenerEntities();

        private GenericRepository<Url> _urlRepository;
        public GenericRepository<Url> UrlRepository
        {
            get
            {
                if (this._urlRepository == null)
                {
                    this._urlRepository = new GenericRepository<Url>(_context);
                }
                return _urlRepository;
            }
        }


        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}