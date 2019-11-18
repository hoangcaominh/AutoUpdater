# AutoUpdater
 C# class library for implementing self-updating application. This project is heavily based on SharpUpdate.
 
# Implement
 - Add this class reference to your desired project
 - Implement the interface:
#### C#: 
```C#
using AutoUpdater;
...
public partial class MyForm : Form, IAutoUpdater
```
#### C++/CLI:
```C++
using namespace AutoUpdater;
...
public ref class MyForm: public Form, public IAutoUpdater
```
