#pragma once

namespace TestProject {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Reflection;
	using namespace AutoUpdater;

	/// <summary>
	/// Summary for MyForm
	/// </summary>
	public ref class MyForm : public System::Windows::Forms::Form, public IAutoUpdater
	{
	public:
		MyForm(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			this->lbVersion->Text = this->ApplicationAssembly->GetName()->Version->ToString();
			updater = gcnew Updater(this);
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~MyForm()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::Button^ btnUpdate;
	private: System::Windows::Forms::Label^ lbVersion;
	protected:

	protected:

	private: Updater^ updater;

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->btnUpdate = (gcnew System::Windows::Forms::Button());
			this->lbVersion = (gcnew System::Windows::Forms::Label());
			this->SuspendLayout();
			// 
			// btnUpdate
			// 
			this->btnUpdate->Location = System::Drawing::Point(150, 226);
			this->btnUpdate->Name = L"btnUpdate";
			this->btnUpdate->Size = System::Drawing::Size(122, 23);
			this->btnUpdate->TabIndex = 0;
			this->btnUpdate->Text = L"Check for update";
			this->btnUpdate->UseVisualStyleBackColor = true;
			this->btnUpdate->Click += gcnew System::EventHandler(this, &MyForm::BtnUpdate_Click);
			// 
			// lbVersion
			// 
			this->lbVersion->AutoSize = true;
			this->lbVersion->Location = System::Drawing::Point(13, 13);
			this->lbVersion->Name = L"lbVersion";
			this->lbVersion->Size = System::Drawing::Size(35, 13);
			this->lbVersion->TabIndex = 1;
			this->lbVersion->Text = L"label1";
			// 
			// MyForm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(284, 261);
			this->Controls->Add(this->lbVersion);
			this->Controls->Add(this->btnUpdate);
			this->Name = L"MyForm";
			this->Text = L"MyForm";
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
		
#pragma region AutoUpdater
	public: virtual property System::String^ ApplicationName
	{
		System::String^ get()
		{
			return L"TestProject";
		}
	}

	public: virtual property System::String^ ApplicationId
	{
		System::String^ get()
		{
			return L"TestProject";
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

	public: virtual property System::Uri^ UpdateConfigLocation
	{
		System::Uri^ get()
		{
			Uri^ uri = gcnew Uri(L"file:///D:/TestProject/update.xml");
			return uri;
		}
	}

	public: virtual property System::Windows::Forms::Form^ Context
	{
		System::Windows::Forms::Form^ get()
		{
			return this;
		}
	}
#pragma endregion

	private: System::Void BtnUpdate_Click(System::Object^ sender, System::EventArgs^ e)
	{
		updater->Update();
	}
};
}
