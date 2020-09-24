
function SearchBooks(InputWord, NumberPage, GenreeId) {
    $.ajax({
                    type: "GET",
                    url: '/SearchBook/getData',

        data: {
            word: InputWord, page: NumberPage, IdGenre: GenreeId
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

