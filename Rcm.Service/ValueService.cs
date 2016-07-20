using Rcm.Core.Domain;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public partial class ValueService {

        #region Fields

        private readonly ValueRepository _valueRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ValueService() {
            this._valueRepository = new ValueRepository();
        }

        #endregion

        #region Methods

        public virtual List<ActValue> GetAllValues(bool aic, bool aoc, bool dic, bool doc) {
            var values = new List<ActValue>();

            if(aic)
                values.AddRange(_valueRepository.GetAI());

            if(aoc)
                values.AddRange(_valueRepository.GetAO());

            if(dic)
                values.AddRange(_valueRepository.GetDI());

            if(doc)
                values.AddRange(_valueRepository.GetDO());

            return values;
        }

        public virtual List<ActValue> GetValues(int pid, bool aic, bool aoc, bool dic, bool doc) {
            var values = new List<ActValue>();

            if(aic)
                values.AddRange(_valueRepository.GetAIP(pid));

            if(aoc)
                values.AddRange(_valueRepository.GetAOP(pid));

            if(dic)
                values.AddRange(_valueRepository.GetDIP(pid));

            if(doc)
                values.AddRange(_valueRepository.GetDOP(pid));

            return values;
        }

        #endregion

    }
}
