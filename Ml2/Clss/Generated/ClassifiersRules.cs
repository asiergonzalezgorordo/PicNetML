// ReSharper disable once CheckNamespace
namespace Ml2.Clss
{
  public class ClassifiersRules
  {
    private readonly Runtime rt;    
    public ClassifiersRules(Runtime rt) { this.rt = rt; }   

    /// <summary>
    /// Class for building and using a simple decision table majority
    /// classifier.<br/><br/>For more information see: <br/><br/>Ron Kohavi: The Power of
    /// Decision Tables. In: 8th European Conference on Machine Learning, 174-189,
    /// 1995.<br/><br/>Options:<br/><br/>-S &lt;search method specification&gt; = 	Full
    /// class name of search method, followed<br/>	by its options.<br/>	eg:
    /// "weka.attributeSelection.BestFirst -D 1"<br/>	(default
    /// weka.attributeSelection.BestFirst)<br/>-X &lt;number of folds&gt; = 	Use cross validation to evaluate
    /// features.<br/>	Use number of folds = 1 for leave one out CV.<br/>	(Default
    /// = leave one out CV)<br/>-E &lt;acc | rmse | mae | auc&gt; = 	Performance
    /// evaluation measure to use for selecting attributes.<br/>	(Default = accuracy
    /// for discrete class and rmse for numeric class)<br/>-I = 	Use nearest
    /// neighbour instead of global table majority.<br/>-R = 	Display decision table
    /// rules.<br/><br/><br/>Options specific to search method
    /// weka.attributeSelection.BestFirst: = <br/>-P &lt;start set&gt; = 	Specify a starting set of
    /// attributes.<br/>	Eg. 1,3,5-7.<br/>-D &lt;0 = backward | 1 = forward | 2 =
    /// bi-directional&gt; = 	Direction of search. (default = 1).<br/>-N &lt;num&gt; =
    /// 	Number of non-improving nodes to<br/>	consider before terminating
    /// search.<br/>-S &lt;num&gt; = 	Size of lookup cache for evaluated
    /// subsets.<br/>	Expressed as a multiple of the number of<br/>	attributes in the data set. (default
    /// = 1)
    /// </summary>
    public DecisionTable DecisionTable { get {
      return new DecisionTable(rt); 
    } }

    /// <summary>
    /// This class implements a propositional rule learner, Repeated Incremental
    /// Pruning to Produce Error Reduction (RIPPER), which was proposed by William
    /// W. Cohen as an optimized version of IREP. <br/><br/>The algorithm is
    /// briefly described as follows: <br/><br/>Initialize RS = {}, and for each class
    /// from the less prevalent one to the more frequent one, DO: <br/><br/>1.
    /// Building stage:<br/>Repeat 1.1 and 1.2 until the descrition length (DL) of the
    /// ruleset and examples is 64 bits greater than the smallest DL met so far, or
    /// there are no positive examples, or the error rate >= 50%. <br/><br/>1.1.
    /// Grow phase:<br/>Grow one rule by greedily adding antecedents (or conditions)
    /// to the rule until the rule is perfect (i.e. 100% accurate). The procedure
    /// tries every possible value of each attribute and selects the condition with
    /// highest information gain: p(log(p/t)-log(P/T)).<br/><br/>1.2. Prune
    /// phase:<br/>Incrementally prune each rule and allow the pruning of any final
    /// sequences of the antecedents;The pruning metric is (p-n)/(p+n) -- but it's
    /// actually 2p/(p+n) -1, so in this implementation we simply use p/(p+n) (actually
    /// (p+1)/(p+n+2), thus if p+n is 0, it's 0.5).<br/><br/>2. Optimization
    /// stage:<br/> after generating the initial ruleset {Ri}, generate and prune two
    /// variants of each rule Ri from randomized data using procedure 1.1 and 1.2. But
    /// one variant is generated from an empty rule while the other is generated by
    /// greedily adding antecedents to the original rule. Moreover, the pruning
    /// metric used here is (TP+TN)/(P+N).Then the smallest possible DL for each
    /// variant and the original rule is computed. The variant with the minimal DL is
    /// selected as the final representative of Ri in the ruleset.After all the rules
    /// in {Ri} have been examined and if there are still residual positives, more
    /// rules are generated based on the residual positives using Building Stage
    /// again. <br/>3. Delete the rules from the ruleset that would increase the DL
    /// of the whole ruleset if it were in it. and add resultant ruleset to RS.
    /// <br/>ENDDO<br/><br/>Note that there seem to be 2 bugs in the original ripper
    /// program that would affect the ruleset size and accuracy slightly. This
    /// implementation avoids these bugs and thus is a little bit different from Cohen's
    /// original implementation. Even after fixing the bugs, since the order of
    /// classes with the same frequency is not defined in ripper, there still seems
    /// to be some trivial difference between this implementation and the original
    /// ripper, especially for audiology data in UCI repository, where there are
    /// lots of classes of few instances.<br/><br/>Details please
    /// see:<br/><br/>William W. Cohen: Fast Effective Rule Induction. In: Twelfth International
    /// Conference on Machine Learning, 115-123, 1995.<br/><br/>PS. We have compared this
    /// implementation with the original ripper implementation in aspects of
    /// accuracy, ruleset size and running time on both artificial data "ab+bcd+defg"
    /// and UCI datasets. In all these aspects it seems to be quite comparable to the
    /// original ripper implementation. However, we didn't consider memory
    /// consumption optimization in this
    /// implementation.<br/><br/><br/><br/>Options:<br/><br/>-F &lt;number of folds&gt; = 	Set number of folds for REP<br/>	One fold
    /// is used as pruning set.<br/>	(default 3)<br/>-N &lt;min. weights&gt; =
    /// 	Set the minimal weights of instances<br/>	within a split.<br/>	(default
    /// 2.0)<br/>-O &lt;number of runs&gt; = 	Set the number of runs
    /// of<br/>	optimizations. (Default: 2)<br/>-D = 	Set whether turn on the<br/>	debug mode
    /// (Default: false)<br/>-S &lt;seed&gt; = 	The seed of randomization<br/>	(Default:
    /// 1)<br/>-E = 	Whether NOT check the error rate>=0.5<br/>	in stopping criteria
    /// 	(default: check)<br/>-P = 	Whether NOT use pruning<br/>	(default: use
    /// pruning)
    /// </summary>
    public JRip JRip { get {
      return new JRip(rt); 
    } }

    /// <summary>
    /// Generates a decision list for regression problems using
    /// separate-and-conquer. In each iteration it builds a model tree using M5 and makes the "best"
    /// leaf into a rule.<br/><br/>For more information see:<br/><br/>Geoffrey
    /// Holmes, Mark Hall, Eibe Frank: Generating Rule Sets from Model Trees. In:
    /// Twelfth Australian Joint Conference on Artificial Intelligence, 1-12,
    /// 1999.<br/><br/>Ross J. Quinlan: Learning with Continuous Classes. In: 5th Australian
    /// Joint Conference on Artificial Intelligence, Singapore, 343-348,
    /// 1992.<br/><br/>Y. Wang, I. H. Witten: Induction of model trees for predicting
    /// continuous classes. In: Poster papers of the 9th European Conference on Machine
    /// Learning, 1997.<br/><br/>Options:<br/><br/>-N = 	Use unpruned
    /// tree/rules<br/>-U = 	Use unsmoothed predictions<br/>-R = 	Build regression tree/rule
    /// rather than a model tree/rule<br/>-M &lt;minimum number of instances&gt; = 	Set
    /// minimum number of instances per leaf<br/>	(default 4)
    /// </summary>
    public M5Rules M5Rules { get {
      return new M5Rules(rt); 
    } }

    /// <summary>
    /// Class for building and using a 1R classifier; in other words, uses the
    /// minimum-error attribute for prediction, discretizing numeric attributes. For
    /// more information, see:<br/><br/>R.C. Holte (1993). Very simple
    /// classification rules perform well on most commonly used datasets. Machine Learning.
    /// 11:63-91.<br/><br/>Options:<br/><br/>-B &lt;minimum bucket size&gt; = 	The
    /// minimum number of objects in a bucket (default: 6).
    /// </summary>
    public OneR OneR { get {
      return new OneR(rt); 
    } }

    /// <summary>
    /// Class for generating a PART decision list. Uses separate-and-conquer.
    /// Builds a partial C4.5 decision tree in each iteration and makes the "best"
    /// leaf into a rule.<br/><br/>For more information, see:<br/><br/>Eibe Frank, Ian
    /// H. Witten: Generating Accurate Rule Sets Without Global Optimization. In:
    /// Fifteenth International Conference on Machine Learning, 144-151,
    /// 1998.<br/><br/>Options:<br/><br/>-C &lt;pruning confidence&gt; = 	Set confidence
    /// threshold for pruning.<br/>	(default 0.25)<br/>-M &lt;minimum number of
    /// objects&gt; = 	Set minimum number of objects per leaf.<br/>	(default 2)<br/>-R =
    /// 	Use reduced error pruning.<br/>-N &lt;number of folds&gt; = 	Set number of
    /// folds for reduced error<br/>	pruning. One fold is used as pruning
    /// set.<br/>	(default 3)<br/>-B = 	Use binary splits only.<br/>-U = 	Generate unpruned
    /// decision list.<br/>-J = 	Do not use MDL correction for info gain on numeric
    /// attributes.<br/>-Q &lt;seed&gt; = 	Seed for random data shuffling (default
    /// 1).
    /// </summary>
    public PART PART { get {
      return new PART(rt); 
    } }

    /// <summary>
    /// Class for building and using a 0-R classifier. Predicts the mean (for a
    /// numeric class) or the mode (for a nominal
    /// class).<br/><br/>Options:<br/><br/>-D = 	If set, classifier is run in debug mode and<br/>	may output
    /// additional info to the console
    /// </summary>
    public ZeroR ZeroR { get {
      return new ZeroR(rt); 
    } }

    
  }
}