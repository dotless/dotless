class Account
{
	decimal balance;
	public void Withdraw(decimal amount) {
		lock (this) {
			if (amount > balance) {
				throw new Exception("Insufficient funds");
			}
			balance -= amount;
		}
	}
}


