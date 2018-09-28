using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ITargetable
{
    // Dep this
    bool IsTargetable();
    // bool IsTargetable { get; set; }
    void SetupTargetIndicator(TargetIndicator indicator);
    event TargetEventHandler BecameTargetable;
    event TargetEventHandler BecameUntargetable;
}

public delegate void TargetEventHandler(ITargetable sender);