from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.views import LoginView
from django.contrib.auth.models import User, Group
from django.shortcuts import redirect
from django.views.generic.edit import CreateView
from django.urls import reverse_lazy

from .forms import UserLoginForm, UserRegistrationForm, ProfileSetUpForm


class UserSignUpView(CreateView):
    form_class = UserRegistrationForm
    success_url = reverse_lazy("profile_set_up")
    template_name = 'registration/signup.html'

    def form_valid(self, form):
        instance = form.save()
        my_group = Group.objects.get(name='Customer')
        my_group.user_set.add(instance)
        self.request.session['user_id'] = instance.id
        return super().form_valid(form)


class UserLoginView(LoginView):
    authentication_form = UserLoginForm
    success_url = reverse_lazy('products')

    # def get(self, req, *args, **kwargs):
    #     if req.user.is_anonymous:
    #         return super().get(req, *args, **kwargs)
    #     else:
    #         return redirect(req.path_info)


class ProfileSetUpView(CreateView):
    form_class = ProfileSetUpForm
    success_url = reverse_lazy('login')
    template_name = 'registration/profile_set_up.html'

    def form_valid(self, form):
        user_id = self.request.session.get('user_id')
        user = User.objects.get(id=user_id)
        form.instance.user = user
        return super().form_valid(form)
