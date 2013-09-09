// ReSharper disable once CheckNamespace
namespace Ml2.Clss
{
  public class ClassifiersTreesLmt
  {
    private readonly Runtime rt;    
    public ClassifiersTreesLmt(Runtime rt) { this.rt = rt; }   

    /// <summary>
    /// No class description found.
    /// </summary>
    public LogisticBase LogisticBase { get {
      return new LogisticBase(rt); 
    } }

    
  }
}