using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data;
using Ramses.Data;
using System.Data.SqlClient;

namespace Ramses.Business
{
    /// <summary>
    /// Classe base para implementação das de regras de negócios que utilizam entidades do contexto
    /// </summary>
    /// <typeparam name="T">Uma entidade disponível através do contexto RamsesDBEntities</typeparam>
    /// <History>
    /// <Created Date="2014/07/06" Author="BPlus">Jackson Antonio do Prado Lima</Created>
    /// </History>
    public abstract class BaseBusiness<T> where T : EntityObject
    {
        private RamsesDBEntities _context;
        private ObjectSet<T> _objectSet;

        /// <summary>
        /// Provides access to functionality and manipulation of entities based application
        /// </summary>
        protected RamsesDBEntities Context
        {
            get
            {
                if (_context == null)
                    _context = new RamsesDBEntities();

                return _context;
            }
        }

        /// <summary>
        /// Fornece acesso e funcionalidades para manipulação da entidade <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">Uma entidade disponível em Context do tipo EntityObject</typeparam>
        protected ObjectSet<T> ObjectSet
        {
            get
            {
                if (_objectSet == null)
                    _objectSet = (ObjectSet<T>)typeof(RamsesDBEntities).GetProperty(this.Context.GetEntitySetName(typeof(T))).GetValue(this.Context, null);

                return _objectSet;
            }
        }

        protected abstract string OnGetErrorMessage(ErrorType error, Exception ex);

        /// <summary>
        /// Exclui um ou mais registros em Context.
        /// </summary>
        /// <param name="predicate">Determina a condição de seleção do dados para exclusão</param>
        /// <typeparam name="T">Uma entidade disponível em Context do tipo EntityObject</typeparam>
        protected void Delete(Func<T, bool> predicate)
        {
            try
            {
                ObjectSet.Where(predicate)
                    .ToList()
                    .ForEach(d => ObjectSet.DeleteObject(d));

                if (this.Context.HasPendingChanges())
                    this.Context.SaveChanges();
            }
            catch (UpdateException ex)
            {
                SqlException sqlEx = ex.InnerException as SqlException;
                if (sqlEx != null && sqlEx.Number == 547)  //Equals ORA-02292
                    throw new BizException(OnGetErrorMessage(ErrorType.IntegrityConstraintDelete, ex), sqlEx);

                throw;
            }
        }

        /// <summary>
        /// Inclui ou altera os dados de obj em Context.
        /// </summary>
        /// <param name="obj">Objeto corrente com os dados para inclusão ou alteração</param>
        /// <param name="predicate">Determina a condição para verificar quando incluir ou alterar um objeto</param>
        public virtual void Save(T obj)
        {
            try
            {
                if (obj.EntityState == EntityState.Detached)
                {
                    object entity;
                    if (Context.TryGetObjectByKey(Context.CreateEntityKey(obj), out entity))
                        ObjectSet.ApplyCurrentValues(obj);
                    else
                        ObjectSet.AddObject(obj);
                }

                if (Context.HasPendingChanges())
                    this.Context.SaveChanges();
            }
            catch (UpdateException ex)
            {
                SqlException sqlEx = ex.InnerException as SqlException;
                if (sqlEx != null && sqlEx.Number == 2627)  //Equal ORA-00001: unique constraint violated
                    throw new BizException(OnGetErrorMessage(ErrorType.UniqueConstraint, ex), sqlEx);

                throw;
            }
        }

        /// <summary>
        /// Retorna uma lista com todos os dados da entidade <typeparamref name="T" />
        /// </summary>
        /// <returns></returns>
        /// <typeparam name="T">Uma entidade definida para a instância corrente de BaseBusiness</typeparam>
        public List<T> GetAll()
        {
            return (from o in ObjectSet select o).ToList();
        }

        /// <summary>
        /// Queries elements that matches the especified filtering criteria
        /// </summary>
        /// <param name="filter">Provides a predicate to filters a sequence of items</param>
        /// <returns></returns>
        /// <typeparam name="T">An entity defined for this instance</typeparam>
        public List<T> GetAll(IEntityFilter<T> filter)
        {
            return GetAll(filter.Predicate);
        }

        /// <summary>
        /// Queries elements that matches the especified filtering criteria
        /// </summary>
        /// <param name="predicate">Filters a sequence of items based on a predicate</param>
        /// <returns></returns>
        /// <typeparam name="T">An entity defined for this instance</typeparam>
        public List<T> GetAll(Func<T, bool> predicate)
        {
            return ObjectSet.Where(predicate).ToList();
        }

        /// <summary>
        /// Queries elements that matches the especified filtering criteria
        /// </summary>
        /// <param name="predicate">Filters a sequence of items based on a predicate</param>
        /// <returns></returns>
        /// <typeparam name="T">An entity defined for this instance</typeparam>
        public T Get(Func<T, bool> predicate)
        {
            return ObjectSet.Where(predicate).FirstOrDefault();
        }
    }
}
