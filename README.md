# CarShop

This is a simple Car Shop project for practice.

Only works on localhost

Used packages:

  	django v.4.1.3
  
  	Pillow 9.3.0
  
  	django-cleanup 6.0.0
  
  	django-crispy-forms 1.14.0
  
  	crispy-bootstrap5 0.7
  
  	django-debug-toolbar 3.7.0
  
 
Implemented the following features:

  	-User registration
  
  	-User Log in
  
  	-Simple site administration/moderation:
  
    	-Adding products
    
    	-Deleting products
    
    	-Product editing
    
  	-Profile (Connected to the default django User via a One-to-One relationship):
  
    	-Profile editing/deleting
    
  	-Watchlist (Profile model connected with Car model via Many-to-Many relationship, with defalut django 'through' model):
		
			-add to watchlist
			
			-delete from watchlist
			
			
P.S. I developed this project in parallel with learning Django. And after a deeper understanding of the possibilities of the language, I see that some parts of the code could have been written shorter and simpler. Also, because of this, the code in the project is stored randomly in illogical places.
  
