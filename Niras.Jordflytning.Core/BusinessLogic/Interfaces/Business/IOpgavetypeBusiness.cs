using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface IOpgavetypeBusiness : IGenericBusiness<Opgavetype>
    {

        IEnumerable<Opgavetype> ForKommune(Guid kommuneId);

        /// <summary>
        /// Angiver om opgavetypen (takst) er gyldig på den angivne dato ud fra FraDato/TilDato (begge inklusiv).
        /// </summary>
        bool ErGyldigPaaDato(Opgavetype opgavetype, DateTime dato);

    }
}
