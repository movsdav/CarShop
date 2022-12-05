from django.urls import path, include
from django.contrib.auth.views import LoginView

from authApp.views import UserSignUpView
from .views import UserLoginView, ProfileSetUpView

urlpatterns = [
    path('login/', UserLoginView.as_view(redirect_authenticated_user=True), name='login'),
    path('register/', UserSignUpView.as_view(), name='register'),
    path('register/profile', ProfileSetUpView.as_view(), name='profile_set_up'),
    path('', include('django.contrib.auth.urls')),
]
