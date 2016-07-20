using Rcm.Core.Caching;
using Rcm.Core.Domain;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public partial class OrderService {

        #region Fields

        private readonly OrderRepository _orderRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public OrderService() {
            this._orderRepository = new OrderRepository();
            this._cacheManager = new CacheManager();
        }

        #endregion

        #region Methods

        public virtual void SetOrder(Order order) {
            _orderRepository.SetEntities(new List<Order>() { order });
        }

        public virtual void SetOrders(List<Order> orders) {
            _orderRepository.SetEntities(orders);
        }

        #endregion

    }
}
