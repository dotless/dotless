#define Debug       // Debugging on
#undef Trace       // Tracing off
class PurchaseTransaction
{
	void Commit() {
		#if Debug
			CheckConsistency();
			#if Trace
				WriteToLog(this.ToString());
			#endif
		#endif
		CommitHelper();
	}
}