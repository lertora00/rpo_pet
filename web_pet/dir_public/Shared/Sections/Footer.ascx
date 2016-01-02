<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Footer.ascx.vb" Inherits="dir_public_Shared_Sections_Footer" %>
<div id="r">
	<div class="container">
		<div class="row centered">
			<div class="col-lg-8 col-lg-offset-2">
				<h4>
                    PetNarrative
				</h4>
				<p>
                    PO Box 1234, Oldville, VA 
				</p>
			</div>
		</div>
	</div>
</div>
<div id="f">
	<div class="container">
		<div class="row centered">
			<a href="#">
                <i class="fa fa-twitter">
                </i>
			</a>
            <a href="#">
                <i class="fa fa-facebook">
                </i>
            </a>
            <a href="#">
                <i class="fa fa-dribbble">
                </i>
            </a>
		</div>
	</div>
</div>
<!-- MODAL FOR CONTACT -->
<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
	<div class="modal-dialog">
	    <div class="modal-content">
	        <div class="modal-header">
	            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
	            <h4 class="modal-title" id="myModalLabel">
                    contact us
	            </h4>
	        </div>
	        <div class="modal-body">
		        <div class="row centered">
		            <p>
                        We are available 24/7, so don't hesitate to contact us.
		            </p>
		            <p>
		        	    123 Your Address 
                        <br/>
					    Somewhere, VA USA.
                        <br/>
					     (800) 555-1212
                        <br/>
					    your.email@your.site.com  
		            </p>
		            <div id="mapwrap">
	                    <iframe height="300" width="100%" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" 
                            src="https://www.google.com/maps?t=m&amp;ie=UTF8&amp;ll=72.752693,142.791016&amp;spn=-7.34552,106.972656&amp;z=2&amp;output=embed">
	                    </iframe>
				    </div>	
		        </div>
	        </div>
	        <div class="modal-footer">
	            <button type="button" class="btn btn-danger" data-dismiss="modal">
                    Save & Go
	            </button>
	        </div>
	    </div>
	</div>
</div>
