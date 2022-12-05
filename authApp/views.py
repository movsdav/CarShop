from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.views import LoginView
from django.contrib.auth.models import User, Group
from django.shortcuts import redirect, get_object_or_404
from django.views.generic.edit import CreateView, UpdateView
from django.urls import reverse_lazy

from .forms import UserLoginForm, UserRegistrationForm, ProfileSetUpForm
from .models import UserProfile


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


class ProfileSetUpView(CreateView):
    form_class = ProfileSetUpForm
    success_url = reverse_lazy('login')
    template_name = 'registration/profile_set_up.html'

    def form_valid(self, form):
        user_id = self.request.session.get('user_id')
        user = User.objects.get(id=user_id)
        form.instance.user = user
        return super().form_valid(form)


class ProfileUpdateView(UpdateView):
    model = UserProfile
    form_class = ProfileSetUpForm
    success_url = reverse_lazy('profile')
    template_name = 'registration/profile_set_up.html'

    def get_object(self, queryset=None):
        return get_object_or_404(UserProfile, pk=self.request.user)
