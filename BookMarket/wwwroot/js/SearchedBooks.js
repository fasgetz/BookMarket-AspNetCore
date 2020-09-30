
function SearchBooks(InputWord, NumberPage, GenreeId, RatingOrdered) {
    
    $("#contentBooks").append('<p class="text-center" id="load">Loading...</p>');

    $.ajax({
                    type: "GET",
                    url: '/SearchBook/getData',

        data: {
            word: InputWord, page: NumberPage, IdGenre: GenreeId, RatingOrdered: RatingOrdered
                    },

                    success: function (data) {
                        $("#contentBooks").html(data);
                        $("#inputWord").val(InputWord);
                        

                        $('html, body').animate({
                            scrollTop: $("html").offset().top
                        });
                    }
                });

}