
function SearchBooks(InputWord) {
    $.ajax({
                    type: "GET",
                    url: '/SearchBook/getData',

                    data: {
                        word: InputWord
                    },

                    success: function (data) {
                        $("#contentBooks").html(data);
                        

                        $('html, body').animate({
                            scrollTop: $("html").offset().top
                        });
                    }
                });

}

