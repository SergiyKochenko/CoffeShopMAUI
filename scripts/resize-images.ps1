param(
    [string]$ImagesPath = (Join-Path $PSScriptRoot '../CoffeShopMAUI/Resources/Images'),
    [int]$MaxDimension = 1200
)

Add-Type -AssemblyName System.Drawing

$resolvedPath = Resolve-Path -Path $ImagesPath

foreach ($file in Get-ChildItem $resolvedPath -Filter *.jpg) {
    $image = $null
    $graphics = $null
    $resized = $null

    try {
        $image = [System.Drawing.Image]::FromFile($file.FullName)
        if ($image.Width -le $MaxDimension -and $image.Height -le $MaxDimension) {
            $image.Dispose()
            $image = $null
            continue
        }

        $scale = [Math]::Min($MaxDimension / $image.Width, $MaxDimension / $image.Height)
        $newWidth = [Math]::Max([int]([Math]::Round($image.Width * $scale)), 1)
        $newHeight = [Math]::Max([int]([Math]::Round($image.Height * $scale)), 1)

        $resized = New-Object System.Drawing.Bitmap($newWidth, $newHeight)
        $graphics = [System.Drawing.Graphics]::FromImage($resized)
        $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.DrawImage($image, 0, 0, $newWidth, $newHeight)

        $image.Dispose()
        $image = $null

        $codec = [System.Drawing.Imaging.ImageCodecInfo]::GetImageEncoders() | Where-Object { $_.MimeType -eq 'image/jpeg' }
        $encoderParameters = New-Object System.Drawing.Imaging.EncoderParameters 1
        $qualityParam = New-Object System.Drawing.Imaging.EncoderParameter([System.Drawing.Imaging.Encoder]::Quality, 85L)
        $encoderParameters.Param[0] = $qualityParam

        $resized.Save($file.FullName, $codec, $encoderParameters)
    }
    finally {
        if ($graphics) { $graphics.Dispose() }
        if ($resized) { $resized.Dispose() }
        if ($image) { $image.Dispose() }
    }
}
