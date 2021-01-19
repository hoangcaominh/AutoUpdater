# AutoUpdater
 C# class library for implementing self-updating application. This project is heavily based on SharpUpdate.
 
# Implement
 - Add this class reference to your desired project
 - Implement the interface:
#### C++/CLI: 
```C++
using namespace AutoUpdater;
...
public ref class MyForm: public Form, public IAutoUpdater
{
	public: virtual property System::Version^ ApplicationVersion
	{
		System::Version^ get()
		{
			return gcnew Version(L"");
		}
	}

	public: virtual property System::Reflection::Assembly^ ApplicationAssembly
	{
		System::Reflection::Assembly^ get()
		{
			return System::Reflection::Assembly::GetExecutingAssembly();
		}
	}

	public: virtual property System::Drawing::Icon^ ApplicationIcon
	{
		System::Drawing::Icon^ get()
		{
			return this->Icon;
		}
	}

	public: virtual property System::Uri^ UpdateInfoLocation
	{
		System::Uri^ get()
		{
			// MAKE SURE THERE IS A SLASH AT THE END OF THE URI OR IT WILL BE CORRUPTED
			return gcnew Uri(L"");
		}
	}

	public: virtual property System::Windows::Forms::Form^ Context
	{
		System::Windows::Forms::Form^ get()
		{
			return this;
		}
	}
}

```
#### C#:
```C#
using AutoUpdater;
...
public partial class MyForm : Form, IAutoUpdater
{
	public Version ApplicationVersion
	{
		get { return ""; }
	}
	
	public Assembly ApplicationAssembly
	{
		get { return Assembly.GetExecutingAssembly(); }
	}
	
	public Icon ApplicationIcon
	{
		get { return this.Icon; }
	}
	
	public Uri UpdateInfoLocation
	{
		// MAKE SURE THERE IS A SLASH AT THE END OF THE URI OR IT WILL BE CORRUPTED
		get { return new Uri(""); }
	}
	
	public Form Context
	{
		get { return this; }
	}
}

```