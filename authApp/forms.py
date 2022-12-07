from django import forms
from django.contrib.auth.forms import AuthenticationForm, UserCreationForm
from django.contrib.auth.models import User

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Submit
from django.forms import ModelForm

from authApp.models import UserProfile


class UserLoginForm(AuthenticationForm):
    def __init__(self, *args, **kwargs):
        super(UserLoginForm, self).__init__(*args, **kwargs)
        self.helper = FormHelper()
        self.helper.add_input(Submit('submit', 'Login', css_class='btn btn-primary login-button'))
        self.helper.form_method = 'POST'


class UserRegistrationForm(UserCreationForm):
    def __init__(self, *args, **kwargs):
        super(UserRegistrationForm, self).__init__(*args, **kwargs)
        self.helper = FormHelper(self)
        self.helper.add_input(Submit('submit', 'Register', css_class='btn btn-primary login-button'))
        self.helper.form_method = 'POST'

    class Meta:
        model = User
        fields = ['username', 'password1', 'password2']


class ProfileSetUpForm(ModelForm):
    def __init__(self, *args, **kwargs):
        super(ProfileSetUpForm, self).__init__(*args, **kwargs)
        self.helper = FormHelper(self)
        self.helper.add_input(Submit('submit', 'Save', css_class='btn btn-primary'))
        self.helper.form_method = 'POST'

    first_name = forms.CharField(max_length=32)
    last_name = forms.CharField(max_length=32)

    field_order = ['first_name', 'last_name', 'avatar']

    class Meta:
        model = UserProfile
        # fields = ['avatar']
        exclude = ['user', 'watch_list']
