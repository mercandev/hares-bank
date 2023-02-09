using System;
namespace HB.Service.Const
{
	public static class ReceiptConst
	{
		public const string RECEIPT_HTML = @"
			<!DOCTYPE html>
				<html>
					<head>
						<meta charset=""utf-8"" />
							<title>Hares Bank Dekont</title>
		<style>
			.invoice-box {
				max-width: 800px;
				margin: auto;
				padding: 30px;
				border: 1px solid #eee;
				box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
				font-size: 16px;
				line-height: 24px;
				font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
				color: #555;
			}

			.invoice-box table {
				width: 100%;
				line-height: inherit;
				text-align: left;
			}

			.invoice-box table td {
				padding: 5px;
				vertical-align: top;
			}

			.invoice-box table tr td:nth-child(2) {
				text-align: right;
			}

			.invoice-box table tr.top table td {
				padding-bottom: 20px;
			}

			.invoice-box table tr.top table td.title {
				font-size: 45px;
				line-height: 45px;
				color: #333;
			}

			.invoice-box table tr.information table td {
				padding-bottom: 40px;
			}

			.invoice-box table tr.heading td {
				background: #eee;
				border-bottom: 1px solid #ddd;
				font-weight: bold;
			}

			.invoice-box table tr.details td {
				padding-bottom: 20px;
			}

			.invoice-box table tr.item td {
				border-bottom: 1px solid #eee;
			}

			.invoice-box table tr.item.last td {
				border-bottom: none;
			}

			.invoice-box table tr.total td:nth-child(2) {
				border-top: 2px solid #eee;
				font-weight: bold;
			}

			@media only screen and (max-width: 600px) {
				.invoice-box table tr.top table td {
					width: 100%;
					display: block;
					text-align: center;
				}

				.invoice-box table tr.information table td {
					width: 100%;
					display: block;
					text-align: center;
				}
			}

			/** RTL **/
			.invoice-box.rtl {
				direction: rtl;
				font-family: Tahoma, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
			}

			.invoice-box.rtl table {
				text-align: right;
			}

			.invoice-box.rtl table tr td:nth-child(2) {
				text-align: left;
			}
		</style>
	</head>

	<body>
		<div class=""invoice-box"">
			<table cellpadding=""0"" cellspacing=""0"">
				<tr class=""top"">
					<td colspan=""2"">
						<table>
							<tr>
								<td class=""title"">
									<img src=""https://user-images.githubusercontent.com/22862224/217651446-79be3d61-ed29-4137-b499-71d8a815f171.png"" style=""width: 100%; max-width: 300px"" />
								</td>

								<td>
									<h3>DEKONT</h3>
									İşlem Numarası #:{0}<br />
									Oluşturma Tarihi: {1}<br />
								</td>
							</tr>
						</table>
					</td>
				</tr>

				<tr class=""information"">
					<td colspan=""2"">
						<table>
							<tr>

								<td>
									<b>Gönderici Hesap:</b>
									<br>
									{2}<br />
									{3}<br />
								</td>

								<td>
									<b>Alıcı Hesap:</b>
									<br>
									{4}<br />
									{5}<br />
								</td>
							</tr>
						</table>
					</td>
				</tr>

				<tr class=""heading"">
					<td>İşlem Bilgileri</td>
					<td>Tutar</td>
				</tr>

				<tr class=""details"">
					<td>{6}</td>
					<td>{7}</td>
				</tr>
			</table>
			<br>
			<br>
			<hr>
			<center>
				<p>Dikkat! Oluşturulan dekontlar gerçek değildir. Bu işlem tamamen örnek bir geliştirmedir!</p>
			</center>
			</div>
			</body>
			</html>";





	}
}

