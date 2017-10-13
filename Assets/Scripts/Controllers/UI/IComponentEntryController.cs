using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IComponentEntryController
{
    void Initialise(IComponentController i);

    void ActivatePoweredControls();
    
}

