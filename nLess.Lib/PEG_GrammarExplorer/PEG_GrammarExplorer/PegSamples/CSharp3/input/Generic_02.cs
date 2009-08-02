class B<T> where T: IEnumerable {}
class D<T>: B<T> where T: IEnumerable {}
class E<T>: B<List<T>> {}