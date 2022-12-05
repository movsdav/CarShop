from django.contrib.auth.models import User
from django.shortcuts import render, redirect
from django.http.response import HttpResponse
from django.views import View


# Create your views here.
class ProfileView(View):
    template_name = 'myApp/profile.html'
    context = {

    }

    def get(self, req, *args, **kwargs):
        profile = self.request.user.userprofile
        self.context['profile'] = profile

        watch_list = profile.watch_list.all()
        self.context['watch_list'] = watch_list
        return render(req, self.template_name, self.context)


class DeleteProfileView(View):
    def get(self, req, *args, **kwargs):
        user_id = req.GET.get('id')
        if req.user.id == user_id:
            user = User.objects.get(id=user_id)
            user.delete()
            return redirect('login')
        else:
            return redirect("profile")
