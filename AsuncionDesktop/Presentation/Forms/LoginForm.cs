using AsuncionDesktop.Application.UseCases;
using AsuncionDesktop.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsuncionDesktop.Presentation.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthLoginUseCase _authLoginUseCase;

        public LoginForm()
        {
            InitializeComponent();
            var authService = new AuthService();
            _authLoginUseCase = new AuthLoginUseCase(authService);
        }

        private async void cmdLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPassword.Text;

            try
            {
                var authResponse = await _authLoginUseCase.ExecuteAsync(username, password);

                if (authResponse != null)
                {
                    MessageBox.Show($"Login successful! Token: {authResponse.Token}");
                    this.Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
